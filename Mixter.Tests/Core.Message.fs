namespace Mixter.Tests.Domain.Core.Message

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

module ``MessageId should`` =
    [<Fact>] 
    let ``Return unique id when generate`` () =
        let messageId1 = MessageId.Generate()
        let messageId2 = MessageId.Generate()

        test <@ messageId1 <> messageId2 @>

module ``Given a Message`` =
    [<Fact>] 
    let ``When quack, then user quacked event is returned`` () =
        let messageId = MessageId.Generate()
        let userId = { Email = "clem@mix-it.fr"}

        test <@ quack messageId userId "hello world" 
                  = [ MessageQuacked { MessageId = messageId; AuthorId = userId; Content = "hello world" } ] @>

    [<Fact>] 
    let ``When requack, then user requacked event is returned`` () =
        let messageId = MessageId.Generate()
        let requaker = { Email = "someone@mix-it.fr"}

        let result =
            [ MessageQuacked { MessageId = messageId; AuthorId = { Email = "clem@mix-it.fr" }; Content = "hello world" } ]
            |> apply
            |> requack requaker

        test <@ result = [ MessageRequacked { MessageId = messageId; Requacker = requaker } ] @>
            
    [<Fact>] 
    let ``When author requack, then nothing is returned`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "clem@mix-it.fr" }

        let result = 
            [ MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" } ]
            |> apply 
            |> requack authorId

        test <@ result |> Seq.isEmpty @>
            
    [<Fact>] 
    let ``When requack two times same message, then nothing is returned`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "author@mix-it.fr" }
        let requackerId = { Email = "requacker@mix-it.fr" }

        let result =
            [ 
                MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" }
                MessageRequacked { MessageId = messageId; Requacker = requackerId }
            ]   
            |> apply 
            |> requack requackerId
            
        test <@ result |> Seq.isEmpty @>
            
    [<Fact>] 
    let ``When delete, then return MessageDeleted`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "author@mix-it.fr" }

        let result =
            [ 
                MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" }
            ]   
            |> apply 
            |> delete authorId
        
        test <@ result = [ MessageDeleted { MessageId = messageId; Deleter = authorId } ] @>
            
    [<Fact>] 
    let ``When delete by someone else than author, then do not return MessageDeleted`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "author@mix-it.fr" }
        let deleterId = { Email = "deleter@mix-it.fr" }

        let result =
            [ 
                MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" }
            ]   
            |> apply 
            |> delete deleterId
        
        test <@ result |> Seq.isEmpty @>
            
    [<Fact>] 
    let ``Given deleted message When delete Then Nothing`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "author@mix-it.fr" }

        let result =
            [ 
                MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" }
                MessageDeleted { MessageId = messageId; Deleter = authorId }
            ]   
            |> apply 
            |> delete authorId

        test <@ result |> Seq.isEmpty @>
            
    [<Fact>] 
    let ``Given deleted message When requack Then do not raise MessageRequacked`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "author@mix-it.fr" }
        let requacker = { Email = "otherUser@mix-it.fr" }

        let result =
            [ 
                MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" }
                MessageDeleted { MessageId = messageId; Deleter = authorId }
            ]   
            |> apply 
            |> requack requacker
            
        test <@ result |> Seq.isEmpty @>
