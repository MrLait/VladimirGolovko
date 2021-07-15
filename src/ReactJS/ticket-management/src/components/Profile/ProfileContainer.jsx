import React from "react";
import Preloader from "../../common/Preloaders/Preloader";
import {
    deposit, editEmail,
    editFirstName, editLanguage, editPassword,
    editSurname, editTimeZoneOffset,
    getBalance, getUserProfile, toggleIsFetching
} from "../../redux/profile-reducer";
import Profile from "./Profile";
import {connect} from "react-redux";
import {ifNotAuthRedirectToHome} from "../../hoc/ifNotAuthRedirectToHome";

class ProfileContainer extends React.Component {
    componentDidMount() {
        this.props.getUserProfile()
    }
    render() {
        return <>
            {this.props.isFetching ? <Preloader /> : <Profile {...this.props}/>}
        </>
    }
}

let mapStateToProps = (state) => {
    return {
        isAuth: state.authPage.isAuth,
        clickingInProgress: state.profilePage.clickingInProgress,
        isFetching: state.profilePage.isFetching,
        balance: state.profilePage.balance,
        firstName: state.profilePage.firstName,
        surname: state.profilePage.surname,
        email: state.profilePage.email,
        timeZoneOffSet: state.profilePage.timeZoneOffSet,
        isNewPassword: state.profilePage.isNewPassword,
        language: state.profilePage.language,
    }
}
let AuthRedirectComponent = ifNotAuthRedirectToHome(ProfileContainer)
export default connect(
    mapStateToProps,
    {
        toggleIsFetching, getBalance, deposit, editSurname,
        getUserProfile, editFirstName, editEmail, editPassword,
        editTimeZoneOffset, editLanguage
        }) (AuthRedirectComponent);