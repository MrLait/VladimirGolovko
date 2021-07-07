import React from "react";
import {Field, reduxForm} from "redux-form";
import {connect} from "react-redux";
import {login} from "../../redux/auth-reducer";
import {Redirect} from "react-router";
import {required} from "../Utils/validators";
import {Input} from "../../common/FormsControls/FormsControls";
import style from "./../../common/FormsControls/FormsControls.module.css"
import {useTranslation} from "react-i18next";


const LoginForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <div>
                <Field placeholder={"Email"} name={"email"}
                       validate={[required]}
                       component={Input}/>
            </div>
            <div>
                <Field placeholder={"Password"} name={"password"}
                       validate={[required]}
                       component={Input} type={'password'}/>
            </div>
            <div>
                <Field component={"input"} name={"rememberMe"} type={"checkbox"}/> remember me
            </div>
            {props.error && <div className={style.formSummaryError}>{t(props.error)}</div>}
            <div>
                <button>Login</button>
            </div>

            <p>
                eventManager: <b>eventManager@gmail.com</b> Password: <b>_Aa123456</b>
            </p>
            <p>
                user: <b>firstUser@gmail.com</b> Password: <b>_Aa123456</b>
            </p>
        </form>
    )
}

const LoginReduxForm = reduxForm(
    {
        form: 'login'
    }
)(LoginForm)

const Login = (props) => {
    const onSubmit = (formData) => {
        console.log(formData);
        props.login(formData.email, formData.password, formData.rememberMe);
    }
    if (props.isAuth) {
        return <Redirect to={'home'}/>
    }
    return <div>
        <h1>Login</h1>
        <LoginReduxForm onSubmit={onSubmit}/>
    </div>
}
const mapStateToProps = (state) => (
    {
        isAuth: state.authPage.isAuth
    }
);

export default connect(mapStateToProps,
    {
        login
    }
)(Login);