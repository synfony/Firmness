# Etapa 1: Build
FROM node:20-alpine AS build
WORKDIR /app

# Copiar primero los archivos de dependencias e instalar
COPY package.json package-lock.json ./
# Usar npm ci con --legacy-peer-deps para ignorar conflictos
RUN npm ci --legacy-peer-deps

# Copiar el resto de los archivos de la aplicación
COPY . .

# Construir la aplicación usando el script sin Turbopack
RUN npm run build:docker

# Etapa 2: Producción
FROM node:20-alpine AS production
WORKDIR /app

ENV NODE_ENV=production

# Copiar los archivos de la compilación desde la etapa de build
COPY --from=build /app/public ./public
COPY --from=build /app/.next/standalone ./
COPY --from=build /app/.next/static ./.next/static

EXPOSE 3000
ENV PORT 3000

# Iniciar el servidor de Next.js
CMD ["node", "server.js"]
