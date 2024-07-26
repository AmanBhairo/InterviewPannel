import { Interviewer } from "./interviewer.model";
import { Timeslot } from "./timeslot.model";

export interface InterviewSlots{
    slotId: number,
    employeeId: number,
    slotDate:string,
    timeslotId:number,
    isBooked:boolean,
    employee:Interviewer,
    timeslot:Timeslot,
}