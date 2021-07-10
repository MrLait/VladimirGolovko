import React from "react";
import Header from "./Header";
import {connect} from "react-redux";
import {initAuthDataFromLocalStorage, logout} from "../../redux/auth-reducer";
import {editLanguage, getUserProfile} from "../../redux/profile-reducer";

class HeaderContainer extends React.Component {
    componentDidMount() {
        this.props.initAuthDataFromLocalStorage(this.props.token);
        this.props.getUserProfile();
    }

    render() {
        return <Header {...this.props}/>
    }
}

let mapStateToProps = (state) => ({
    isAuth: state.authPage.isAuth,
    email: state.authPage.email,
    language: state.profilePage.language,
});

export default connect(mapStateToProps,
    {initAuthDataFromLocalStorage, logout,
        editLanguage,getUserProfile})(HeaderContainer);