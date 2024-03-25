# Github Explorer

[![Build status](https://ci.appveyor.com/api/projects/status/kpkcc3urwdbv0819?svg=true)](https://ci.appveyor.com/project/KonH/githubexplorer)
[![codecov](https://codecov.io/gh/KonH/GithubExplorer/branch/master/graph/badge.svg)](https://codecov.io/gh/KonH/GithubExplorer)
[![Nuget](https://img.shields.io/nuget/v/GithubExplorer.CommandLine)](https://www.nuget.org/packages/GithubExplorer.CommandLine)

## Description

Simple terminal wrapper to retrieve small information subset using Github API and serialize it.
This tool can be installed as .NET Core Global Tool, manual build is not required.

## Installation

```
dotnet tool install --global GithubExplorer.CommandLine
```

## Usage

### Access token

You should have environment variable with GitHub access token with name `GH_ACCESS_TOKEN`.

### Repositories

```
github_explorer --target repositories --username %USER% --output repositories.json
```
```
info: GithubExplorer.Explorer[0]
      Retrieving user information for '...'
info: GithubExplorer.Explorer[0]
      Retrieving repositories for '...'
info: GithubExplorer.Explorer[0]
      Found ... repositories for '...'
info: GithubExplorer.Writer[0]
      Result saved into 'repositories.json'
```

=> repositories.json:
```
[
  {
    "Url": "https://api.github.com/repos/...",
    "HtmlUrl": "https://github.com/...",
    "CloneUrl": "https://github.com/...",
    "GitUrl": "git://github.com/...",
    "SshUrl": "git@github.com:...",
    "SvnUrl": "https://github.com/...",
    "MirrorUrl": null,
    "Id": ...,
    "NodeId": "MDEwOlJlcG9zaXRvcnkxNDI3NjY0NjY=",
    "Owner": {
        ...
    },
    "Name": "...",
    ...
  },
  ...
]
```

### Pull requests

**Attention!** Filter here is required, serialization breaks without specifying safe properties (at least not selecting StringEnum based ones).

```
github_explorer -t pullrequests -u %USER% -o pull_requests.json --filter "Title;CreatedAt"
```
```
info: GithubExplorer.Explorer[0]
      Found ... pull requests for '...'
info: GithubExplorer.Writer[0]
      Result saved into 'pull_requests.json'
```
=> pull_requests.json:
```
[
  {
    "Title": "Circular buffer yields on single-core machines",
    "CreatedAt": "2020-10-29T23:49:30+00:00"
  },
  ...
]
```
