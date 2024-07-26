export interface ChangePassword{
    email : string | null | undefined;
    oldPassword : string;
    newPassword : string;
    newConfirmPassword : string;
}