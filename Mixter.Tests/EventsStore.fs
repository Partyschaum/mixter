namespace Mixter.Tests.Infrastructure.EventsStore

open Xunit
open Swensen.Unquote
open System
open Mixter.Infrastructure.EventsStore

type Event =
    | EventA of string
    | EventB of string

type ``MemoryEventsStore tests`` ()=
    [<Fact>] 
    member x.``When store events of aggregate and get events for this aggregate Then return these events`` () =
        let eventsStore = new MemoryEventsStore()

        let aggregateId = "AggregateA"
        let events = [ EventA "A"; EventB "B"; EventA "C"]
        
        events |> eventsStore.Store aggregateId

        test <@ eventsStore.Get<Event> aggregateId = events @>

    [<Fact>] 
    member x.``Given events for an aggregate When store events for this aggregate and get events Then return all events`` () =
        let eventsStore = new MemoryEventsStore()
        let aggregateId = "AggregateA"
        let oldEvents = [ EventA "A"; EventB "B"; EventA "C"]
        oldEvents |> eventsStore.Store aggregateId

        let newEvents = [ EventA "D"; EventB "E"]
        newEvents |> eventsStore.Store aggregateId

        test <@ eventsStore.Get<Event> aggregateId = (oldEvents @ newEvents) @>

    [<Fact>] 
    member x.``Given events for an aggregate When store events for another aggregate and get events for this aggregate Then return only events of this aggregate`` () =
        let eventsStore = new MemoryEventsStore()
        let aggregateIdA = "AggregateA"
        let aggregateIdB = "AggregateB"
        let eventsOfAggregateA = [ EventA "A"; EventB "B"; EventA "C"]
        eventsOfAggregateA |> eventsStore.Store aggregateIdA

        let eventsOfAggregateB = [ EventA "D"; EventB "E"]
        eventsOfAggregateB |> eventsStore.Store aggregateIdB

        test <@ eventsStore.Get<Event> aggregateIdB = eventsOfAggregateB @>
        
    [<Fact>] 
    member x.``When get events for unknown aggregate Then return no event`` () =
        let eventsStore = new MemoryEventsStore()

        test <@ eventsStore.Get<Event> "Unknown" = [] @>
