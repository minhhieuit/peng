FROM node:22-alpine AS base
WORKDIR /app

FROM base AS deps
COPY package*.json ./
RUN npm ci

FROM base AS build
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build

FROM base AS runner
COPY --from=build /app/.output ./.output
ENV NODE_ENV=production
ENV NUXT_PUBLIC_API_BASE=http://api:8080
EXPOSE 3000
CMD ["node", ".output/server/index.mjs"]
