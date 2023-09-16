#!/usr/bin/bash

(
    cd ../
    docker build -t codechallenge -t felipe/codechallenge . 
    docker push felipe/codechallenge:latest

    cd setup
    docker-compose rm -f
    docker-compose down --rmi all
    docker-compose up --build
)