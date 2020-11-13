# Github Explorer

## Description

Simple terminal wrapper to retrieve small information subset using Github API and serialize it.

## Usage

### Repositories

```
cd GithubExplorer.CommandLine
dotnet run --target repositories --username %USER% --output repositories.json
```

=> repositories.json:
```

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
```