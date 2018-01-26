﻿module Mixter.Infrastructure.Core

open Mixter.Domain.Core.Timeline

open System.Collections.Generic

type MemoryTimelineMessageStore() =
    let store = new HashSet<TimelineMessage>()

    member __.Save timelineMessage =
        store.Add timelineMessage |> ignore

    member __.GetMessagesOfUser userId =
        store |> Seq.filter (fun p -> p.Owner = userId)

    member __.Delete messageId =
        store.RemoveWhere(fun p -> p.MessageId = messageId) |> ignore
