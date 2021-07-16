import React from "react";
import {Field, reduxForm} from "redux-form";
import {connect} from "react-redux";
import {login} from "../../redux/auth-reducer";
import {Redirect} from "react-router";
import {required} from "../Utils/validators";
import {Input} from "../../common/FormsControls/FormsControls";
import style from "./../../common/FormsControls/FormsControls.module.css"
import {useTranslation} from "react-i18next";
import {formNames} from "../Constants/formNames";
import {ifAuthRedirectToHome} from "../../hoc/ifAuthRedirectToHome";
import {compose} from "redux";


const LoginForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <div>
                <Field placeholder={t("Email")} name={"email"}
                       validate={[required]}
                       component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Password")} name={"password"}
                       validate={[required]}
                       component={Input} type={'password'}/>
            </div>
            <div>
                <Field component={"input"} name={"rememberMe"} type={"checkbox"}/> {t("RememberMe")}
            </div>
            {props.error && <div className={style.formSummaryError}>{t(props.error)}</div>}
            <div>
                <button>{t('Login')}</button>
            </div>

            <p>
                eventManager: <b>eventManager@gmail.com</b> {t("Password")}: <b>_Aa123456</b>
            </p>
            <p>
                user: <b>firstUser@gmail.com</b> {t("Password")}: <b>_Aa123456</b>
            </p>
        </form>
    )
}

const LoginReduxForm = reduxForm(
    {
        form: formNames.Login
    }
)(LoginForm)

const Login = (props) => {
    const {t} = useTranslation();
    const onSubmit = (formData) => {
        console.log(formData);
        props.login(formData.email, formData.password, formData.rememberMe);
    }

    return <div>
        <h1>{t('Login')}</h1>
        <LoginReduxForm onSubmit={onSubmit}/>
    </div>
}
const mapStateToProps = (state) => ({});

export default compose(
    connect(mapStateToProps,{login}),
    ifAuthRedirectToHome
)(Login)