# Pastr Server - ASPNet Core

> This is the backend code for the Pastr service. For frontend go to [this](https://github.com/pastr-git/client) repository.

Api info paste -> https://pastr.userr00t.com/api_info.md

## Build Status
|Branch|Status|
|---|---|
|[master](https://github.com/pastr-git/server/blob/master/)|[![CircleCI](https://circleci.com/gh/pastr-git/server/tree/master.svg?style=svg)](https://circleci.com/gh/pastr-git/server/tree/master)|

## Endpoints
> {BASE} = Base API Url - https://pastr.userr00t.com/api

### GET {BASE}/paste/{id}
Fetches one paste by ID.
 
> `{BASE}/paste/dummy`

Expected status code: `200`
Expected Content Type: `application/json; charset=utf-8`
Expected output:
```json
{
  "id":"dummy",
  "title":"Dummy Response",
  "content":"This is a dummy response used for testing.",
  "date":"2019-08-29T14:44:28.377Z"
}
```
 
### GET {BASE}/paste/{id}/raw
Fetches the raw content of one paste by ID.
 
> `{BASE}/paste/dummy/raw`

Expected status code: `200`
Expected Content Type: `text/plain; charset=utf-8`
Expected output:
```
This is a dummy response used for testing.
```
 
### POST {BASE}/paste/
Creates a new post.
 
> `{BASE}/paste/dummy`

Expected status code: `200`
Expected Content Type: `application/json; charset=utf-8`
Expected output:
```json
{
  "id":"dummy",
  "title":"Dummy Response",
  "content":"This is a dummy response used for testing.",
  "date":"2019-08-29T14:44:28.377Z"
}
```
 
### PUT {BASE}/paste/
Updates existing paste. editCode and ID must be provided. Other fields will be overwritten.
 
> `{BASE}/paste/dummy`

Expected status code: `200`
 
### DELETE {BASE}/paste/
Deletes existing paste. editCode and ID must be provided. Paste will be fully deleted.
 
> `{BASE}/paste/dummy`

Expected status code: `204`

___

## Todo
- [ ] Unit tests
