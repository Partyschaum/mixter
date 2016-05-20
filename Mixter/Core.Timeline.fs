module Mixter.Domain.Core.Timeline

open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

type TimelineMessage = { Owner: UserId; Author: UserId; Content: string; MessageId: MessageId }

let handle save remove evt =
    match evt with
    | MessageQuacked e -> save { Owner = e.UserId; Author = e.UserId; Content = e.Content; MessageId = e.MessageId }
    | MessageDeleted e -> remove e.MessageId
    | _ -> ()