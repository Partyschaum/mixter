module Mixter.Tests.Domain.Identity.UserIdentity

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity.UserIdentity;

[<TestFixture>]
type ``Given a User`` ()=

    [<Test>] 
    member x.``When he registers, then user registered event is returned`` () =
        register ({ Email = "clem@mix-it.fr" }) 
            |> should equal [ UserRegistered { UserId = { Email = "clem@mix-it.fr"} } ]