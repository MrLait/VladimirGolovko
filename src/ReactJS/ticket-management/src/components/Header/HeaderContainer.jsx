import React from "react";
import Header from "./Header";
import {connect} from "react-redux";
import {initAuthDataFromLocalStorage, logout} from "../../redux/auth-reducer";

class HeaderContainer extends React.Component {
    componentDidMount() {
        this.props.initAuthDataFromLocalStorage(this.props.token);
    }

    render() {
        return <Header {...this.props}/>
    }
}

let mapStateToProps = (state) => ({
    isAuth: state.authPage.isAuth,
    email: state.authPage.email,

});

export default connect(mapStateToProps, {initAuthDataFromLocalStorage, logout})(HeaderContainer);