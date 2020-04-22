#!/bin/sh
ls -1 *.unity|sed -e s/".unity"/","/g > scenelist.txt
