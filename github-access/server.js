const fetch = require('node-fetch');
//const logger = require('elasticsearch-logger')();
const createLogger = require('elasticsearch-logger');
 
const logger = createLogger({
    host: "docker06:9200",
    application: "github-access-checker",
    index: "workers"
})

function fetchGitHub() { 
  logger.info({ message: `Start request to GitHub.` })
  fetch('https://github.com/')
    .then(res => {
      logger
        .info({ message: `Successfully recieved a response from GitHub. Status ${res.status}` })
        .then(() => console.info(`Successfully recieved a response from GitHub. Status ${res.status}`));
    })
    .catch(err => {
      logger
        .error({ error: err })
        .then(() => console.error(err));
    });
}

setInterval(fetchGitHub, 5*60*1000);

//start
fetchGitHub();