#!/usr/bin/env bash

# shovel_person_created_error here is the name of the shovel
rabbitmqctl set_parameter shovel shovel_person_created_error '{ "src-uri":"amqp://", "src-queue":"person_created_error", "dest-uri": "amqp://", "dest-queue": "person_created" }'
