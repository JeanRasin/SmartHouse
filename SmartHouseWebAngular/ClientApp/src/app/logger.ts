export class EventId {
  StateId: number;
  Name: string
}

export class Logger {
  LogLevel: number;
  EventId: EventId;
  Message: string;
  Id: string
}
