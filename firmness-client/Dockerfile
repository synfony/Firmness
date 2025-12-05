# Build stage
FROM node:20 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
# Install Pinia, which is a required dependency
RUN npm install pinia
# Install Axios, which is a required dependency
RUN npm install axios
COPY . .
# Set the API URL for the production build
ARG VITE_API_BASE_URL=http://localhost:8081/api
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}
RUN npm run build

# Production stage
FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html
# Copy a custom Nginx config to handle SPA routing
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]