# TikTokCategoryExtractor

This repository contains a program that interacts with the TikTok API and performs various operations based on the provided command. The available commands are:

- GenerateReport
- GenerateEnum
- GenerateUnorderedList
- FindMatchingKeys
- BuildCategoryBreadCrumbs
- GenerateFieldDescriptions
- RemoveDuplicateProducts

## Prerequisites

To run this program, make sure you have the following prerequisites installed:

- .NET Framework 4.7.2 or higher
- Newtonsoft.Json package

## Installation

1. Clone this repository to your local machine.
2. Open the solution in your preferred IDE.
3. Install the Newtonsoft.Json NuGet package.
4. Build the solution.

## Configuration

Before running the program, you need to provide the necessary configuration values. Open the `Program.cs` file and locate the `InitializeProperties` method. Update the following properties with your own values:

- `_apiVersion`: The TikTok API version.
- `_baseURI`: The base URI for the TikTok API.
- `_accessToken`: Your TikTok API access token.
- `_appKey`: Your TikTok API app key.
- `_appSecret`: Your TikTok API app secret.
- `_fileName`: The name of the output file.
- `_filePath`: The path where the output file will be saved.

## Usage

To use the program, run the compiled executable or start it from your IDE. It will execute the command specified in the `_command` variable.

- For the `GenerateReport` command, the program will retrieve TikTok categories and generate a report based on the response.
- For the `GenerateEnum` command, the program will generate an enum based on the provided input.
- For the `GenerateUnorderedList` command, the program will convert a list into an HTML unordered list and save it to a file.
- For the `FindMatchingKeys` command, the program will find matching keys in the product attributes retrieved from the TikTok API.
- For the `BuildCategoryBreadCrumbs` command, the program will build category breadcrumbs based on the TikTok categories.

Make sure to adjust the command and any related input within the `Main` method according to your needs.

## License

This code is released under the MIT License. See the LICENSE file for more details.

Please note that this program interacts with the TikTok API and follows their terms of service.
