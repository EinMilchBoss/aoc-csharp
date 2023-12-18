# About

This repository contains solutions for Advent of Code problems.

Advent of Code is a christmas calender where each day contains two parts of a coding challenge. They get progressively more difficult by the day. If you want to join, just go to: [https://adventofcode.com]().

# How tos

## How to run

`dotnet run --project <project>`

**Example:**

`dotnet run --project Year2023/Day01/Day01.csproj`

## How to add a new day

1. Create a new console project.

`dotnet new console -o <year>/<day>/`

2. Add console project to solution.

`dotnet sln add <year>/<day>/<day>.csproj`

3. Add util dependency to project.

`dotnet add <year>/<day>/<day>.csproj reference Util`
