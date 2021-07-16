import React from "react";
import {Field, reduxForm} from "redux-form";
import {connect} from "react-redux";
import {Redirect} from "react-router";
import {emailShouldContainAt, required} from "../Utils/validators";
import {Input} from "../../common/FormsControls/FormsControls";
import style from "./../../common/FormsControls/FormsControls.module.css"
import {useTranslation} from "react-i18next";
import {register} from "../../redux/auth-reducer";
import {formNames} from "../Constants/formNames";

const RegisterForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <div>
                <Field placeholder={t("User name")} name={"userName"} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("First name")} name={"firstName"} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Surname")} name={"surname"} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Email")} name={"email"} validate={[required, emailShouldContainAt]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Password")} name={"password"} validate={[required]} component={Input}
                       type={'password'}/>
            </div>
            <div>
                <Field placeholder={t("Confirm the password")} name={"passwordConfirm"} validate={[required]}
                       component={Input} type={'password'}/>
            </div>

            {props.error && <div className={style.formSummaryError}>{t(props.error)}</div>}
            <div>
                <button>{t('Register')}</button>
            </div>
        </form>
    )
}

const RegisterReduxForm = reduxForm(
    {
        form: formNames.Register
    }
)(RegisterForm)

const Register = (props) => {
    const {t} = useTranslation();
    const onSubmit = (formData) => {
        props.register(formData.userName, formData.firstName, formData.surname,
            formData.email, formData.password, formData.passwordConfirm)
    }
    if (props.isAuth) {
        return <Redirect to={'home'}/>
    }
    return <div>
        <h1>{t('Register')}</h1>
        <RegisterReduxForm onSubmit={onSubmit}/>
    </div>
}
const mapStateToProps = (state) => (
    {
        isAuth: state.authPage.isAuth
    }
);

export default connect(mapStateToProps,
    {
        register
    }
)(Register);