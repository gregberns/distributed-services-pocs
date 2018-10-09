const fetch = require("node-fetch");

const interval = 800;


const doLoop = () => {
  console.log('Call Started')

  ping();
}

setInterval(doLoop, interval)


const ping = () =>{
  const url = 'http://localhost:6800/ping'  
  const headers = {
    host: 'alpha.localhost'
  }

  fetch(url, { method: 'GET', headers: headers})
    .then(res => {
      res.text().then(t =>{
        console.log(`${new Date().toLocaleTimeString()} Call Finished. Status: ${res.status}, Body: ${t}`)
      })
    })
    .catch(err => {
      console.error("Error occured" + err)
    })
}

const toJson = res => res.json();

