# WichSystem
WichSystem is a .NET tool designed to identify the operating system of a given host based on it's Time To Live (TTL) value.
This project is inspired by : https://github.com/Akronox/WichSystem.py

## How It Works
When you provide an IP address, WichSystem sends a network request and analyzes the TTL in the response. Because different operating systems often set default TTL values, you can make an educated guess about the OS behind that IP.

## Give execute permission
`chmod +x ./WichSystem`

## Use
`./WichSystem 127.0.0.1`

The result will return:
`127.0.0.1 (ttl ->127): Linux`

## Notes & Disclaimer
+ This tool uses heuristic assumptions. Actual TTL values may vary based on network configuration, virtualized environments, or custom settings.
+ Always ensure you have permission to probe the target IP.