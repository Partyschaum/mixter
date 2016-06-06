namespace Mixter.Tests.Infrastructure.EventsStore

open NUnit.Framework
open FsUnit
open System
open Mixter.Infrastructure.EventsStore

type Event =
    | EventA of string
    | EventB of string

[<TestFixture>]
type ``MemoryEventsStore tests`` ()=
    [<Test>] 
    member x.``When store events of aggregate and get events for this aggregate Then return these events`` () =
        let eventsStore = new MemoryEventsStore()

        let aggregateId = "AggregateA"
        let events = [ EventA "A"; EventB "B"; EventA "C"]
        
        events |> eventsStore.Store aggregateId

        eventsStore.Get<Event> aggregateId 
        |> should equal events

    [<Test>] 
    member x.``Given events for an aggregate When store events for this aggregate and get events Then return all events`` () =
        let eventsStore = new MemoryEventsStore()
        let aggregateId = "AggregateA"
        let oldEvents = [ EventA "A"; EventB "B"; EventA "C"]
        oldEvents |> eventsStore.Store aggregateId

        let newEvents = [ EventA "D"; EventB "E"]
        newEvents |> eventsStore.Store aggregateId

        eventsStore.Get<Event> aggregateId 
        |> should equal (oldEvents @ newEvents)

    [<Test>] 
    member x.``Given events for an aggregate When store events for another aggregate and get events for this aggregate Then return only events of this aggregate`` () =
        let eventsStore = new MemoryEventsStore()
        let aggregateIdA = "AggregateA"
        let aggregateIdB = "AggregateB"
        let eventsOfAggregateA = [ EventA "A"; EventB "B"; EventA "C"]
        eventsOfAggregateA |> eventsStore.Store aggregateIdA

        let eventsOfAggregateB = [ EventA "D"; EventB "E"]
        eventsOfAggregateB |> eventsStore.Store aggregateIdB

        eventsStore.Get<Event> aggregateIdB 
        |> should equal eventsOfAggregateB
        
    [<Test>] 
    member x.``When get events for unknown aggregate Then return no event`` () =
        let eventsStore = new MemoryEventsStore()

        eventsStore.Get<Event> "Unknown" 
        |> should equal []