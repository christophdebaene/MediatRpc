# Introduction

The API is an HTTP server based on [JSON-RPC](https://en.wikipedia.org/wiki/JSON-RPC). Note that the [OpenAPI](https://swagger.io/specification/) specification doesn't support [JSON-RPC](https://en.wikipedia.org/wiki/JSON-RPC) calls and therefore some conventions to interpret this documentation.

[JSON-RPC](https://en.wikipedia.org/wiki/JSON-RPC) is a `Post` request with the following payload on endpoint `/jsonrpc`

```
{
	"id" : "A02061B2-589F-40C6-BA98-34726C4516EB",
	"jsonrpc" : "2.0",	
	"method": "MethodName",
	"params" : {	
	}
}
```

Properties

* `id`: Typically a *Guid* used to match the response with the request
* `jsonrpc` : [JSON-RPC](https://en.wikipedia.org/wiki/JSON-RPC) version used, is always `2.0`
* `method`: Each operation has a unique method name that can be found in the description
* `params` : Here comes the *payload* of the operation

Note that each request is a `Post` due to [JSON-RPC](https://en.wikipedia.org/wiki/JSON-RPC)
and that in the documentation the method `Get` is used to denote a **Query** whereas `Post` to denote a **Command**.  

Response of an [JSON-RPC](https://en.wikipedia.org/wiki/JSON-RPC) call looks like

```
{
	"id" : "A02061B2-589F-40C6-BA98-34726C4516EB",
	"jsonrpc" : "2.0",
	"result" : {
	
	}
}
```

Properties

* `id`: The same *id* that was used for the request
* `jsonrpc` : [JSON-RPC](https://en.wikipedia.org/wiki/JSON-RPC) version used, is always `2.0`
* `result`: Returns the json result for that specific operation

When there is an error, a property `error` is returned with the error details.

```
{
	"id" : "",
	"jsonrpc" : "2.0",
	"error" : {
		"code" : "",
		"message" : "",
		"data" : ""
	}
}
```