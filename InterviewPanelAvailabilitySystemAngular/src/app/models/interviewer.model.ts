import { InterviewRoundInterviewer } from "./interviewround.interviewer.model";
import { JobRoleInterviewer } from "./jobrole.interviewer.model";

export interface Interviewer{
    employeeId: number;
    firstName: string;
    lastName: string;
    email: string;
    jobRoleId: number;
    interviewRoundId: number;
    isRecruiter: boolean;
    isAdmin: boolean;
    changePassword: boolean;
    jobRole: JobRoleInterviewer;
    interviewRound: InterviewRoundInterviewer
}