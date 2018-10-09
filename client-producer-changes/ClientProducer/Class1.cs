using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClientProducer
{

    /*
     * Concepts to look for
     *
     *  * Code Depth is minimal 
     *  * Almost every method/function is independently testable
     *  * Easy to 'reason' about what the program is doing
     *  * Each method takes in a value and returns a value, no `out` or voids
     *  * `for` loops are barely used
     */


    public class Class1
    {
        private Func<> _messageSender;
        private Action<Exception> _log;

        public Class1()
        {

        }

        public void Startup()
        {
            var validatedInput = Validate();
            var files = GetFiles(validatedInput.PathToMonitor);

            foreach (var file in files)
            {
                var result = ProcessFile(file);
                if (!result.IsSuccess)
                {

                }

                //To Generic message
                var environment = "";

                //_kafkaConfiguration.Topics
                //ApplicationSettings.EnvironmentLabel
                var messages = 
                    result.Item
                        .Select(m => CreateMessage(m, validatedInput.EnvironmentLabel, validatedInput.Topic))
                        .ToList();

                //Send Message


            }

        }

        // Will need way more validation
        public ProgramData Validate()
        {
            //These values will probably be passed in...
            var pathToMonitor = "";
            

            var errors = new List<string>();

            if (!Directory.Exists(pathToMonitor))
            {
                errors.Add($"PathToMonitor is invalid: {pathToMonitor}");
            }

            var stringFormat = ""; //Config.ApplicationSettings.Instance.MessageFormat;
            //var messageFormat = EnumUtility.ParseMessageFormat(stringFormat);
            string messageformat;
            //if (Enum.TryParse(stringFormat, true,))

            //Kafka Topic
            var topic = "";

            //Environment Label
            var environmentLabel = "";

            return ProgramData.New(
                new DirectoryInfo(pathToMonitor),
                topic,
                environmentLabel
            );
        }

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo dir)
        {
            return dir.GetFiles().OrderBy(p => p.CreationTime.Ticks).ToList();
        }

        public ExceptionalList<IEnumerable<FileMessage>> ProcessFile(FileInfo file)
        {
            var fileResult = ReadFile(file);
            if (!fileResult.IsSuccess)
            {
                return ExceptionalList<IEnumerable<FileMessage>>.Failure(
                    new List<Exception>() { fileResult.Exception });
            }

            var parseResult = ParseFile(fileResult.Item);
            if (parseResult.IsSuccess)
            {
                return ExceptionalList<IEnumerable<FileMessage>>.Failure(
                    new List<Exception>() { parseResult.Exception });
            }

            var messagesResult = ParseMessages(parseResult.Item);
            if (!messagesResult.IsSuccess)
            {
                return ExceptionalList<IEnumerable<FileMessage>>.Failure(messagesResult.Exceptions);
            }

        }

        public Exceptional<string> ReadFile(FileInfo file)
        {
            if (file.Exists)
            {
                return Exceptional<string>.Failure(new Exception("File not found"));
            }

            try
            {
                var contents = File.ReadAllText(file.FullName);
                return Exceptional<string>.Success(contents);
            }
            catch (Exception e)
            {
                //might need to pass a better exception back here because we may not know where it came from otherwise
                return Exceptional<string>.Failure(e);
            }
        }

        public Exceptional<List<MonitoredData>> ParseFile(string fileContents)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<List<MonitoredData>>(fileContents);
                return Exceptional<List<MonitoredData>>.Success(result);
            }
            catch (Exception e)
            {
                return Exceptional<List<MonitoredData>>.Failure(e);
            }
        }

        ExceptionalList<IEnumerable<FileMessage>> ParseMessages(IEnumerable<MonitoredData> data)
        {
            return data.Select(ParseMessage)
                //You may not be familiar with this... it is 'reduce' in javascript
                .Aggregate(
                    //this is the 'accumulator' that gets updated for each item
                    ExceptionalList<IEnumerable<FileMessage>>.Success(new List<FileMessage>()),
                    //this is the function that updates the accumulator
                    (acc, i) =>
                    {
                        if (!i.IsSuccess)
                        {
                            return ExceptionalList<IEnumerable<FileMessage>>.Failure(acc.Exceptions.Append(i.Exception));
                        }
                        if (!acc.IsSuccess)
                        {
                            return ExceptionalList<IEnumerable<FileMessage>>.Failure(acc.Exceptions);
                        }
                        return ExceptionalList<IEnumerable<FileMessage>>.Success(acc.Item.Append(i.Item));
                    });
        }

        public Exceptional<FileMessage> ParseMessage(MonitoredData data)
        {
            var typeResponse = GetMessageType(data.ModelType);
            if (!typeResponse.IsSuccess)
            {
                //need to include the Monitored Data object for error research
                return Exceptional<FileMessage>.Failure(typeResponse.Exception);
            }

            var messageResponse = ReadMessageBody(data.MessageData, typeResponse.Item);
            if (!messageResponse.IsSuccess)
            {
                return Exceptional<FileMessage>.Failure(messageResponse.Exception);
            }

            

            return Exceptional<FileMessage>.Success(
                    FileMessage.New(messageResponse.Item, data.ModelType, data.TopicKey, Convert.ToUInt64(data.UniqueID)));
        }

        public Exceptional<IProducerMessage> ReadMessageBody(string messageData, Type type)
        {
            string decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(messageData));

            try
            {
                var value = (IProducerMessage) JsonConvert.DeserializeObject(decodedString, type);
                return Exceptional<IProducerMessage>.Success(value);
            }
            catch (Exception e)
            {
                return Exceptional<IProducerMessage>.Failure(e);
            }
        }

        public Exceptional<Type> GetMessageType(ModelType type)
        {
            if (type == ModelType.FpaEnrollmentRequest)
            {
                return Exceptional<Type>.Success(typeof(FPAEnrollmentRequest));

            }
            if (type == ModelType.SundanceActivityNotification)
            {
                return Exceptional<Type>.Success(typeof(SundanceAccountActivityList));
            }
            if (type == ModelType.SundanceCustomerUpdate)
            {
                return Exceptional<Type>.Success(typeof(SundanceCustomerUpdate));
            }
            return Exceptional<Type>.Failure(new Exception("Invalid ModelType"));
        }

        private IMessage CreateMessage(FileMessage message, string environmentLabel, string topic)
        {
            //var serializedMessage = _serializer.SerializeMessage(message);
            var _topic = "";//_kafkaConfiguration.Topics;
            var environment = environmentLabel; //ApplicationSettings.EnvironmentLabel;
            
            return new KafkaMessage
            {
                Payload = JsonConvert.SerializeObject(message.Payload),
                Topic = topic,
                MessageHeader = new MessageHeader
                {
                    Environment = environmentLabel,
                    Format = MessageFormat.Json,
                    LoggingGuid = Guid.NewGuid(),
                    UniqueId = message.UniqueId,
                    ModelType = message.ModelType.ToString()
                },
                Key = message.TopicKey
            };
        }
    }

    public class FileMessage
    {
        private FileMessage(
            IProducerMessage payload,
            ModelType modelType,
            string topicKey,
            ulong uniqueId
            )
        {
            Payload = payload;
            ModelType = modelType;
            TopicKey = topicKey;
            UniqueId = uniqueId;
        }

        public static FileMessage New(
            IProducerMessage payload,
            ModelType modelType,
            string topicKey,
            ulong uniqueId
        )
        {
            return new FileMessage(
                payload,
                modelType,
                topicKey,
                uniqueId
            );
        }

        public readonly IProducerMessage Payload;
        public readonly ModelType ModelType;
        public readonly string TopicKey;
        public readonly ulong UniqueId;
    }

    public class InternalMessage
    {
        private InternalMessage(
            string topic,
            string payload,
            string topicKey,
            string environment,
            Guid loggingGuid,
            ulong uniqueId,
            ModelType modelType
            )
        {
            Format = MessageFormat.Json;
            LoggingGuid = loggingGuid;
            UniqueId = uniqueId;
            ModelType = modelType;
        }

        public static InternalMessage New(
            string topic,
            string payload,
            string topicKey,
            string environment,
            Guid loggingGuid,
            ulong uniqueId,
            ModelType modelType
            )
        {
            return new InternalMessage(
                topic,
                payload,
                topicKey,
                environment,
                loggingGuid,
                uniqueId,
                modelType
                );
        }

        public string Topic;
        public string Payload;
        public string Key;

        //Headers
        public readonly string Environment;
        public readonly MessageFormat Format;
        public readonly Guid LoggingGuid;
        public readonly ulong UniqueId;
        public readonly ModelType ModelType;
    }

    //There are way better data structures, but this is good enough and doesnt involve pulling in a new library
    public class Exceptional<T>
    {
        public readonly bool IsSuccess;
        public readonly T Item;
        public readonly Exception Exception;

        private Exceptional(T t)
        {
            IsSuccess = true;
            Item = t;
        }

        private Exceptional(Exception e)
        {
            IsSuccess = false;
            Exception = e;
        }

        public static Exceptional<T> Success(T t)
        {
            return new Exceptional<T>(t);
        }

        public static Exceptional<T> Failure(Exception e)
        {
            return new Exceptional<T>(e);
        }
    }

    public class ExceptionalList<T>
    {
        public readonly bool IsSuccess;
        public readonly T Item;
        public readonly IEnumerable<Exception> Exceptions;

        private ExceptionalList(T t)
        {
            IsSuccess = true;
            Item = t;
        }

        private ExceptionalList(IEnumerable<Exception> e)
        {
            IsSuccess = false;
            Exceptions = e;
        }

        public static ExceptionalList<T> Success(T t)
        {
            return new ExceptionalList<T>(t);
        }

        public static ExceptionalList<T> Failure(IEnumerable<Exception> e)
        {
            return new ExceptionalList<T>(e);
        }
    }

    public class ProgramData
    {
        public readonly DirectoryInfo PathToMonitor;
        public readonly string EnvironmentLabel;
        public readonly string Topic;

        private ProgramData(
            DirectoryInfo pathToMonitor,
            string environmentLabel,
            string topic)
        {
            PathToMonitor = pathToMonitor;
            EnvironmentLabel = environmentLabel;
            Topic = topic;
        }

        public static ProgramData New(
            DirectoryInfo PathToMonitor,
            string EnvironmentLabel,
            string Topic)
        {
            return new ProgramData(PathToMonitor, EnvironmentLabel, Topic);
        }
    }

    public enum ModelType
    {
        FpaEnrollmentRequest,
        SundanceCustomerUpdate,
        SundanceActivityNotification,
        Unknown
    }

    public class MonitoredData
    {
        /// <summary>
        /// ModelType of contained data.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ModelType ModelType { get; set; }
        /// <summary>
        /// Data to be sent to IProducer.
        /// </summary>
        public string MessageData { get; set; }
        public string TopicKey { get; set; }
        public string UniqueID { get; set; }
    }

    public interface IProducerMessage
    {
    }

    public interface IMessage
    {

    }

    //[AssociatedModelType(Type = Enums.ModelType.FpaEnrollmentRequest)]
    public class FPAEnrollmentRequest : IProducerMessage
    {
        //TODO: Determine which fields are actually required always.
        [JsonProperty(Required = Required.Always)]
        public string Sender { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string ProcessingType { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string ContractAccount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CustomerStatus { get; set; }
        [JsonProperty(Required = Required.Always)]
        public DateTime ChargeEffectiveDate { get; set; }
        [JsonProperty(Required = Required.Always)]
        public DateTime ChargeEndDate { get; set; }
        [JsonProperty(Required = Required.Always)]
        public Double ChargeAmount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string PremiseNo { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CompanyCode { get; set; }

    }

    //[AssociatedModelType(Type = Enums.ModelType.SundanceCustomerUpdate)]
    public class SundanceCustomerUpdate : IProducerMessage
    {
        public string OwnerNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CustomerEntityType { get; set; }
        public int Deleted { get; set; }
        public string Operator { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
        //public Address Address { get; set; }
        //public List<Phones> Phones { get; set; }
        //public EmailAddresses EmailAddresses { get; set; }
        //public List<AccountNumberModel> AccountNumbers { get; set; }
    }

    //[AssociatedModelType(Type = Enums.ModelType.SundanceActivityNotification)]
    public class SundanceAccountActivityList : IProducerMessage
    {
      //  public List<AccountActivityModel> List { get; set; }
    }


    public enum MessageFormat
    {
        NotSpecified = 0,
        Json = 1,
        Xml = 2,
        Text = 3,
        Unknown = 99
    }

    public class MessageHeader
    {
        public string ModelType { get; set; }
        public string Environment { get; set; }
        public Guid LoggingGuid { get; set; }
        public string Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageFormat Format { get; set; }
        public ulong UniqueId { get; set; }
        [JsonIgnore]
        public string SerializedMessageHeader
        {
            get
            {
                switch (Format)
                {
                    case MessageFormat.Json:
                        return JsonConvert.SerializeObject(this);
                    case MessageFormat.NotSpecified:
                    case MessageFormat.Xml:
                    case MessageFormat.Text:
                    case MessageFormat.Unknown:
                    default:
                        throw new ArgumentException($"No serialization method specified for type {Format}");
                }
            }
        }
    }

    public class KafkaMessage : IMessage
    {
        public MessageHeader MessageHeader { get; set; }
        public string Topic { get; set; }
        public string Payload { get; set; }
        public string Key { get; set; }
        [JsonIgnore]
        public string SerializedMessage
        {
            get
            {
                switch (MessageHeader.Format)
                {
                    case MessageFormat.Json:
                        return JsonConvert.SerializeObject(this);
                    case MessageFormat.NotSpecified:
                    case MessageFormat.Xml:
                    case MessageFormat.Text:
                    case MessageFormat.Unknown:
                    default:
                        throw new ArgumentException($"No serialization method specified for type {MessageHeader.Format}");
                }
            }
        }
    }

}
