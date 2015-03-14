Allows roundtrip serialization and deserialization of common feed formats.

A complete object model for Atom 1.0/0.3, RSS 2.0, and RSS 1.0 (RDF) plus associated IHttpAsyncHandlers for generating feeds.

Creating a custom feed is as easy as inheriting from Handler, overriding a method which returns the feed object.

Extensible adapter model allows additional extensions (i.e. "modules") such as Dublin Core (provided as an example adapter).

Common feed interface allows reading all feeds with same object model.