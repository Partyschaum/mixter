module Mixter.Domain.Core.Message

open Mixter.Domain.Identity.UserIdentity
open System

type MessageId = MessageId of string
    with static member generate = MessageId (Guid.NewGuid().ToString())

type Event =
    | MessageQuacked of MessageQuacked
    | MessageRequacked of MessageRequacked
and MessageQuacked = { MessageId: MessageId; UserId: UserId; Content: string}
and MessageRequacked = { MessageId: MessageId }

type DecisionProjection = 
    | NotQuackedMessage
    | QuackedMessage of QuackedMessage 
and QuackedMessage = { MessageId: MessageId; AuthorId: UserId; }

let quack messageId authorId content =
    [ MessageQuacked { MessageId = messageId; UserId = authorId; Content = content } ]

let requack requackerId decisionProjection =
    match decisionProjection with
    | QuackedMessage p when p.AuthorId <> requackerId -> [ MessageRequacked { MessageId = p.MessageId } ]
    | QuackedMessage _
    | NotQuackedMessage -> []

let applyOne decisionProjection event =
    match event with
    | MessageQuacked e -> QuackedMessage { MessageId = e.MessageId; AuthorId = e.UserId }
    | MessageRequacked _ -> decisionProjection

let apply events =
    Seq.fold applyOne NotQuackedMessage events

