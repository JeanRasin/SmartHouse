import { EventId } from "./event-id";

export class Logger {
  logLevel: number;
  eventId: EventId;
  message: string;
  id: string;
  date: string;
}
