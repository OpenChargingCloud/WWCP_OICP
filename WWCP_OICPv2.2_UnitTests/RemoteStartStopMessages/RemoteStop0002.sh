#!/bin/bash

curl -v -X POST -d @RemoteStop0002.xml -H "Content-Type: text/xml" http://127.0.0.1:3103/RNs/PROD/Authorization
