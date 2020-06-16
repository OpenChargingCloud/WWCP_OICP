#!/bin/bash

curl -v -X POST -d @RemoteStop0001.xml -H "Content-Type: text/xml" http://127.0.0.1:3103/RNs/PROD/Authorization
