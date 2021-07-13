import {userClaim} from "../../components/Constants/userConst";

export const getCurUserId = () => {
    const localUser = localStorage.getItem('user');
    if (localUser == null) return undefined;
    const parsedUser = JSON.parse(localUser);
    let userCredential = (parsedUser) => ({
        id: parsedUser[userClaim.id],
        name: parsedUser[userClaim.name],
        email: parsedUser[userClaim.email],
        role: parsedUser[userClaim.role],
    });
    let id = userCredential(parsedUser).id;

    return id;
}