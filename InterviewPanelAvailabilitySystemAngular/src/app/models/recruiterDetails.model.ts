import { Interviewer } from "./interviewer.model";


export interface RecruiterDetails{
    slotId: number,
    employeeId: number,
    slotDate:string,
    timeslotId:number,
    isBooked:boolean,
    firstName:string,
    lastName:string,
    email:string,
    jobRoleId:number,
    jobRoleName:string,
    timeslotName:string,
    interviewRoundId:number,
    interviewRoundName:string,
    employee:Interviewer

}