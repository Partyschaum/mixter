module Mixter.Domain.Core.Message

open Mixter.Domain.Identity.UserIdentity
open System

type MessageId = MessageId of string
    with static member generate = MessageId (Guid.NewGuid().ToString())

type Event =
    | MessageQuacked of MessageQuacked
    | MessageRequacked of MessageRequacked
    | MessageDeleted of MessageDeleted
and MessageQuacked = { MessageId: MessageId; UserId: UserId; Content: string}
and MessageRequacked = { MessageId: MessageId; Requacker: UserId }
and MessageDeleted = { MessageId: MessageId; Deleter: UserId }

type DecisionProjection = 
    | NotQuackedMessage
    | QuackedMessage of QuackedMessage
and QuackedMessage = { MessageId: MessageId; AuthorId: UserId; Requackers: UserId list }

let quack messageId authorId content =
    [ MessageQuacked { MessageId = messageId; UserId = authorId; Content = content } ]

let requack requackerId decisionProjection =
    match decisionProjection with
    | QuackedMessage p when p.AuthorId = requackerId -> []
    | QuackedMessage p when p.Requackers |> List.exists (fun r -> r = requackerId) -> []
    | QuackedMessage p -> [ MessageRequacked { MessageId = p.MessageId; Requacker = requackerId } ]
    | NotQuackedMessage -> []

let delete deleter decisionProjection = 
    match decisionProjection with
    | QuackedMessage p when p.AuthorId <> deleter -> []
    | QuackedMessage p -> [ MessageDeleted { MessageId = p.MessageId; Deleter = deleter } ]
    | _ -> []

let applyOne decisionProjection event =
    match event with
    | MessageQuacked e -> QuackedMessage { MessageId = e.MessageId; AuthorId = e.UserId; Requackers = [] }
    | MessageRequacked e -> 
        match decisionProjection with
        | NotQuackedMessage -> decisionProjection
        | QuackedMessage p -> QuackedMessage { p with Requackers = e.Requacker :: p.Requackers }

let apply events =
    Seq.fold applyOne NotQuackedMessage events

