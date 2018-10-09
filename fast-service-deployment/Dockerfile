FROM node:8

# Create app directory
WORKDIR ./src/app

# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
# where available (npm@5+)
COPY package*.json ./

RUN npm install
# If you are building your code for production
# RUN npm install --only=production

# Bundle app source
COPY . .

EXPOSE 3000

# Got this from here
# https://m.mattmclaugh.com/traefik-and-consul-catalog-example-2c33fc1480c0
# https://github.com/mattmcla/traefik-consul/blob/master/Dockerfile
COPY traefik.toml /etc/traefik/traefik.toml


CMD [ "npm", "start" ]