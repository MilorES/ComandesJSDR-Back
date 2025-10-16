#!/bin/bash
set -e

# Simple wait-for-db script using /dev/tcp; waits up to 60s for DB to accept TCP connections
host="${DB_HOST:-mariadb}"
port="${DB_PORT:-3306}"
timeout_seconds=60

echo "Waiting for database at $host:$port (timeout ${timeout_seconds}s)..."
for i in $(seq 1 $timeout_seconds); do
  if (echo > /dev/tcp/${host}/${port}) >/dev/null 2>&1; then
    echo "Database is reachable: $host:$port"
    exec dotnet ComandesAPI.dll
  fi
  sleep 1
done

echo "Timed out waiting for database at $host:$port" >&2
exit 1
