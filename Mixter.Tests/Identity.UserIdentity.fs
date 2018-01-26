module Mixter.Tests.Domain.Identity.UserIdentity

open Xunit
open Swensen.Unquote
open System
open Mixter.Domain.Identity.UserIdentity;

type ``Given a User`` ()=

    [<Fact>] 
    member x.``When he registers, then user registered event is returned`` () =
        test <@ register ({ Email = "clem@mix-it.fr" }) 
                    = [ UserRegistered { UserId = { Email = "clem@mix-it.fr"} } ] @>