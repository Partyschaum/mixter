module Mixter.Tests.Domain.Identity.UserIdentity

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity;

module ``Given a User`` =

    [<Fact>] 
    let ``When he registers, then user registered event is returned`` () =
        test <@ register ({ Email = "clem@mix-it.fr" }) 
                    = [ UserRegistered { UserId = { Email = "clem@mix-it.fr"} } ] @>