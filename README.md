# Rabbit MQ Shovel Example

- A `docker-compose.yml` file to initiate a Rabbit MQ container.

`app/` folder contains:
- An example application that produces messages to a RabbitMQ instance.
- An example application that consumes messages from the RabbitMQ instance (badly).
   - This will be used to populate an "error" queue.

`scripts/` folder contains:
- Scripts related to the RabbitMQ Shovel plugin.
  - 01_enable_shovel.sh
  - 02_start_shovel.sh
  - 03_stop_shovel.sh
  - 99_check_shovel_status.sh

Each of the `.sh` scripts in the `scripts/` folder will be mounted in the RabbitMQ container at the `/scripts` directory.

To run the docker-compose container, do the following
```bash
# Host machine, from root of repo
docker compose pull
docker compose up -d
docker compose exec rabbit bash

# Inside the container
cd /scripts
ls -la
# ... Invoke the scripts as necessary
```

While the docker compose environment is running, the RabbitMQ management interface will be accessible on the host machine.
- URL: `http://localhost:15672`
- Username: `rabbit`
- Password: `rabbit`


## Running the sample:
1. Clone the repo
1. Start the RabbitMQ container
   ```bash
   # In a new terminal window, from root of repo:
   docker compose pull
   docker compose up -d
   docker compose exec rabbit bash

   # In the spawned container
   cd /scripts
   ls -la
   # ... Invoke the scripts as necessary
   ```
1. Start the Consumer project
   ```bash
   # In a new terminal window, from root of repo:
   cd ./app/RabbitMqShovelExample.Consumer
   dotnet run
   ```
   - This will attempt to consume the messages; though they will all be sent to the `person_created_error` queue.
1. Start the Producer project
   ```bash
   # In a new terminal window, from root of repo:
   cd ./app/RabbitMqShovelExample.Producer

   # ... After a few seconds, stop the application.
   CTRL+C
   ```
1. Go back to the terminal containing the Consumer project
   ```bash
   # Stop the project
   CTRL+C
   ```
1. Open the RabbitMQ web interface, login, go to the queues.
   - The two queues should be created: `person_created`, `person_created_error`.
   - All records should be in the `person_created_error` queue.
1. Go back to the terminal containing the RabbitMQ container.
   ```bash
   # Assuming the steps are followed, you will be in /scripts.
   #  ... If not, cd to `/scripts`.

   # Enable the shovel plugin.
   ./01_enable.shovel.sh

   # Start the shovel to move items from `person_created_error` to `person_created`.
   # Important that the consumer is stopped at this point, or it will keep moving items back to the `person_created_error` queue.
   ./02_start_shovel.sh

   # Check the status of the shovel, should see our new shovel.
   ./99_check_shovel_status.sh
   ```
1. Go back to the RabbitMQ web interface, go to queues again.
   - All items should be in the `person_created` queue.
   - The `person_created_error` queue should be empty.
1. Go back to the terminal containing the RabbitMQ container.
   ```bash
   # Stop the shovel plugin.
   # This prevents the shovel indefinitely moving messages from the ERROR queue to the main queue.
   ./03_stop_shovel.sh

   # Check the status of the shovel, should no longer exist.
   ./99_check_shovel_status.sh
   ```