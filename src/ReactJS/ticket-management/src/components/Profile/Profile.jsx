import React from "react";
import BalanceForm from "./Forms/BalanceForm";
import LanguageSelector from "../../common/Selectors/LanguageSelector";
import TimeZoneOffSetSelector from "../../common/Selectors/TimeZoneOffSetSelector";
import EditSurnameForm from "./Forms/EditSurnameForm";
import EditFirstNameForm from "./Forms/EditFirstNameForm";
import EditEmailForm from "./Forms/EditEmailForm";
import EditPasswordForm from "./Forms/EditPasswordForm";
import style from "./Profile.module.css"

let Profile = (props) => {
    return (
        <div>
            <BalanceForm {...props} onSubmit = {(formData) => {props.deposit(formData.balance)}}/>
            <EditSurnameForm {...props} onSubmit = {(formData) => {props.editSurname(formData.surname)}}/>
            <EditFirstNameForm {...props} onSubmit = {(formData) => {props.editFirstName(formData.firstName)}}/>
            <EditEmailForm {...props} onSubmit = {(formData) => {props.editEmail(formData.email)}}/>
            <EditPasswordForm {...props} onSubmit = {(formData) => {props.editPassword(formData.oldPassword, formData.newPassword)}}/>
            <div className={style.selector}>
                <LanguageSelector  {...props}/>
            </div>
            <div className={style.selector}>
                <TimeZoneOffSetSelector {...props}/>
            </div>
        </div>
    )
}

export default Profile;