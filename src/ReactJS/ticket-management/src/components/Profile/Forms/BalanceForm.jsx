import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";
import style from "./Form.module.css"

const BalanceForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td className={style.td}> {t('Balance')} - {props.balance} {t('Currency')} </td>
                    <td>
                        <span>
                            <Field type="number" placeholder={t("Deposit")} name={"balance"}
                                   component={Input}/>
                            <button disabled={props.clickingInProgress}>{t('Deposit')} </button>
                        </span>
                    </td>
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: 'balance'
})(BalanceForm)
