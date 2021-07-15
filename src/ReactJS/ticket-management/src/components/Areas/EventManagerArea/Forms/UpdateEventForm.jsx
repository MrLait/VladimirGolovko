import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {required} from "../../../Utils/validators";
import {Input, renderDatePicker} from "../../../../common/FormsControls/FormsControls";
import {Redirect} from "react-router";
import {formNames} from "../../../Constants/formNames";
import {fieldNames} from "../../../Constants/fieldConst";
import style from "../../../../common/FormsControls/FormsControls.module.css";

let UpdateEventForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <div>
                <Field placeholder={t("Name")} name={fieldNames.name} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Description")} name={fieldNames.description} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field type={'number'} placeholder={t("LayoutId")} name={fieldNames.layoutId} validate={[required]}
                       component={Input}/>
            </div>
            <div>
                <Field placeholder={t("StartDateTime")} name={fieldNames.startDateTime} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("EndDateTime")} name={fieldNames.endDateTime} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("ImageUrl")} name={fieldNames.imageUrl} validate={[required]} component={Input}/>
            </div>

            <div>
                {props.error && <div className={style.formSummaryError}>{t(props.error)}</div>}
                <button disabled={props.inProgress}>
                    {t('UpdateEvent')}
                </button>
                {props.isUpdateEventSuccessful
                    ? <Redirect to={'/eventManagerArea'}/>
                    : ""}
            </div>
        </form>
    )
}

export default reduxForm(
    {
        form: formNames.UpdateEvent
    }
)(UpdateEventForm)