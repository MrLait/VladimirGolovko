import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {required} from "../../../Utils/validators";
import {Input, renderDatePicker} from "../../../../common/FormsControls/FormsControls";
import {Redirect} from "react-router";
import Preloader from "../../../../common/Preloaders/Preloader";

let UpdateEventForm = (props) => {
    const {t} = useTranslation();
    //if (!props.event) return <Preloader/>
    return (
        <form onSubmit={props.handleSubmit}>
            <div>
                <Field placeholder={t("Name")} name={'name'} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Description")} name={"description"} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field type={'number'} placeholder={t("LayoutId")} name={"layoutId"} validate={[required]}
                       component={Input}/>
            </div>
            <div>
                <Field placeholder={t("StartDateTime")} name={"startDateTime"} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("EndDateTime")} name={"endDateTime"} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("ImageUrl")} name={"imageUrl"} validate={[required]} component={Input}/>
            </div>

            <div>
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

UpdateEventForm = reduxForm({
    form: 'updateEvent',
})(UpdateEventForm);


export default UpdateEventForm;

