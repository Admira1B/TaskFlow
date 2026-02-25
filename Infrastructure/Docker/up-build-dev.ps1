$env:ENVIRONMENT="dev"
docker compose --env-file .env --env-file .env.dev up --build