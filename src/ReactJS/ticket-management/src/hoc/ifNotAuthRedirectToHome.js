import {Component} from "react";
import {connect} from "react-redux";
import {Redirect} from "react-router";
import {routes} from "../components/Constants/routes";
import React from "react";

let mapStateToProps = (state) => ({
    isAuth : state.authPage.isAuth
})

export const ifNotAuthRedirectToHome = (Component) =>{
    class RedirectComponent extends React.Component{
        render() {
            if (!this.props.isAuth) return <Redirect to={routes.home.href}/>

            return <Component {...this.props}/>
        }
    }

    let ConnectedAuthRedirect = connect(mapStateToProps)(RedirectComponent)

    return ConnectedAuthRedirect;
}