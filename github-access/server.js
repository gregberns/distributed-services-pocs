const fetch = require('node-fetch');
//const logger = require('elasticsearch-logger')();
const createLogger = require('elasticsearch-logger');
 
const logger = createLogger({
    host: "docker06:9200",
    application: "github-access-checker",
    index: "workers-github"
})

function fetchGitHub() { 
  logger.info({ message: `Start request to GitHub.` })
  fetch('https://github.com/')
    .then(res => {
      console.info(`Successfully recieved a response from GitHub.`, res.status)
      logger
        .info({ message: `Successfully recieved a response from GitHub. Status ${res.status}`, status: res.status })
    })
    .catch(err => {
      console.error('Failed to connect to Github.', err)
      return logger
        //.error(new Error('Failed to connect to Github. Message: ' + JSON.stringify(err)))
        //.error(new Error(JSON.stringify({ message: 'Failed to connect to Github.', error: err })))
        .error(err)
        .catch(err => console.error('Error when logging error:', err))
    });
}

setInterval(fetchGitHub, 5*60*1000);

//start
fetchGitHub();