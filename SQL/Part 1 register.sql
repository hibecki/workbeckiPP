/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     4/5/2016 5:39:17 PM                          */
/*==============================================================*/


execute sp_unbindefault 'm_money'
go

execute sp_unbindefault 'ppercent'
go

if exists (select 1
   from  sysobjects where type = 'D'
   and name = 'd_0'
   )
   drop default d_0
go

execute sp_unbindefault 'code_3'
go

if exists (select 1
   from  sysobjects where type = 'D'
   and name = 'd_00'
   )
   drop default d_00
go

execute sp_unbindefault 'code_30'
go

execute sp_unbindefault 'pic_image.ref_doc_type'
go

if exists (select 1
   from  sysobjects where type = 'D'
   and name = 'd_1'
   )
   drop default d_1
go

execute sp_unbindefault 'status_flag'
go

if exists (select 1
   from  sysobjects where type = 'D'
   and name = 'd_n'
   )
   drop default d_n
go

execute sp_unbindefault 'ini_config.system'
go

if exists (select 1
   from  sysobjects where type = 'D'
   and name = 'd_z'
   )
   drop default d_z
go

create rule r_status_flag as
      @column = upper(@column)
go

/*==============================================================*/
/* Default: d_0                                                 */
/*==============================================================*/
create default d_0
    as 0
go

/*==============================================================*/
/* Default: d_00                                                */
/*==============================================================*/
create default d_00
    as '00'
go

/*==============================================================*/
/* Default: d_1                                                 */
/*==============================================================*/
create default d_1
    as '0'
go

/*==============================================================*/
/* Default: d_n                                                 */
/*==============================================================*/
create default d_n
    as 'N'
go

/*==============================================================*/
/* Default: d_z                                                 */
/*==============================================================*/
create default d_z
    as 'Z'
go

/*==============================================================*/
/* Table: instructor                                            */
/*==============================================================*/
create table instructor (
   instructor_code      varchar(30)          not null,
   instructor_desc      nvarchar(100)        not null,
   confirm_date         datetime             null,
   ref_doc              varchar(30)          null,
   contactor            nvarchar(100)        null,
   contactor_detail     nvarchar(500)        null,
   x_status             char(1)              null
      constraint ckc_x_status_instruct check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_instructor primary key (instructor_code)
)
go

execute sp_bindefault d_1, 'instructor.instructor_code'
go

execute sp_bindefault d_1, 'instructor.ref_doc'
go

execute sp_bindefault d_n, 'instructor.x_status'
go

/*==============================================================*/
/* Table: course_instructor                                     */
/*==============================================================*/
create table course_instructor (
   instructor_code      varchar(30)          not null,
   course_code          varchar(30)          not null,
   confirm_date         datetime             null,
   ref_doc              varchar(30)          null,
   instructor_cost      money                null,
   x_status             char(1)              null
      constraint ckc_x_status_course_i check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_course_instructor primary key (instructor_code, course_code)
)
go

execute sp_bindefault d_1, 'course_instructor.instructor_code'
go

execute sp_bindefault d_1, 'course_instructor.course_code'
go

execute sp_bindefault d_1, 'course_instructor.ref_doc'
go

execute sp_bindefault d_0, 'course_instructor.instructor_cost'
go

execute sp_bindefault d_n, 'course_instructor.x_status'
go

/*==============================================================*/
/* Table: course_group                                          */
/*==============================================================*/
create table course_group (
   cgroup_code          char(3)              not null,
   cgroup_desc          nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_course_g check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_course_group primary key (cgroup_code)
)
go

execute sp_bindefault d_00, 'course_group.cgroup_code'
go

execute sp_bindefault d_n, 'course_group.x_status'
go

/*==============================================================*/
/* Table: course_train_place                                    */
/*==============================================================*/
create table course_train_place (
   place_code           varchar(30)          not null,
   course_code          varchar(30)          not null,
   confirm_date         datetime             null,
   ref_doc              varchar(30)          null,
   place_cost           money                null,
   x_status             char(1)              null
      constraint ckc_x_status_course_t check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_course_train_place primary key (place_code, course_code)
)
go

execute sp_bindefault d_1, 'course_train_place.place_code'
go

execute sp_bindefault d_1, 'course_train_place.course_code'
go

execute sp_bindefault d_1, 'course_train_place.ref_doc'
go

execute sp_bindefault d_0, 'course_train_place.place_cost'
go

execute sp_bindefault d_n, 'course_train_place.x_status'
go

/*==============================================================*/
/* Table: "course_type\"                                        */
/*==============================================================*/
create table "course_type\" (
   cgroup_code          char(3)              not null,
   ctype_code           char(3)              not null,
   ctype_desc           nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_course_t check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint "pk_course_type\" primary key (ctype_code, cgroup_code)
)
go

execute sp_bindefault d_00, '"course_type\".cgroup_code'
go

execute sp_bindefault d_00, '"course_type\".ctype_code'
go

execute sp_bindefault d_n, '"course_type\".x_status'
go

/*==============================================================*/
/* Table: ini_config                                            */
/*==============================================================*/
create table ini_config (
   client_code          varchar(30)          not null,
   system               nvarchar(50)         not null,
   module               nvarchar(50)         not null,
   cnfig_item           nvarchar(50)         not null,
   text_value           nvarchar(500)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_ini_conf check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   numeric              identity,
   rowversion           timestamp            null,
   constraint pk_ini_config primary key (client_code, system, module, cnfig_item)
)
go

execute sp_bindefault d_1, 'ini_config.client_code'
go

execute sp_bindefault d_z, 'ini_config.system'
go

execute sp_bindefault d_n, 'ini_config.x_status'
go

/*==============================================================*/
/* Table: ini_country                                           */
/*==============================================================*/
create table ini_country (
   country_code         int                  not null,
   country_desc         nvarchar(100)        not null,
   area_part            varchar(30)          null,
   x_status             char(1)              null
      constraint ckc_x_status_ini_coun check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_ini_country primary key (country_code)
)
go

execute sp_bindefault d_1, 'ini_country.area_part'
go

execute sp_bindefault d_n, 'ini_country.x_status'
go

/*==============================================================*/
/* Table: ini_district                                          */
/*==============================================================*/
create table ini_district (
   country_code         int                  not null,
   province_code        char(8)              not null,
   district_code        char(8)              not null,
   dist_desc            nvarchar(100)        not null,
   area_part            varchar(30)          null,
   x_status             char(1)              null
      constraint ckc_x_status_ini_dist check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_ini_district primary key (district_code, province_code, country_code)
)
go

execute sp_bindefault d_1, 'ini_district.area_part'
go

execute sp_bindefault d_n, 'ini_district.x_status'
go

/*==============================================================*/
/* Table: ini_list_zip                                          */
/*==============================================================*/
create table ini_list_zip (
   province_code        char(8)              not null,
   country_code         int                  not null,
   district_code        char(8)              not null,
   subistrict_code      char(8)              not null,
   zip_code             char(5)              not null,
   x_status             char(1)              null
      constraint ckc_x_status_ini_list check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_ini_list_zip primary key (province_code, country_code, district_code, subistrict_code, zip_code)
)
go

execute sp_bindefault d_n, 'ini_list_zip.x_status'
go

/*==============================================================*/
/* Table: ini_province                                          */
/*==============================================================*/
create table ini_province (
   country_code         int                  not null,
   province_code        char(8)              not null,
   pro_desc             nvarchar(100)        not null,
   area_part            varchar(30)          null,
   x_status             char(1)              null
      constraint ckc_x_status_ini_prov check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_ini_province primary key (province_code, country_code)
)
go

execute sp_bindefault d_1, 'ini_province.area_part'
go

execute sp_bindefault d_n, 'ini_province.x_status'
go

/*==============================================================*/
/* Table: ini_subdistrict                                       */
/*==============================================================*/
create table ini_subdistrict (
   country_code         int                  not null,
   province_code        char(8)              not null,
   district_code        char(8)              not null,
   subistrict_code      char(8)              not null,
   dist_desc            nvarchar(100)        not null,
   area_part            varchar(30)          null,
   x_status             char(1)              null
      constraint ckc_x_status_ini_subd check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_ini_subdistrict primary key (province_code, country_code, district_code, subistrict_code)
)
go

execute sp_bindefault d_1, 'ini_subdistrict.area_part'
go

execute sp_bindefault d_n, 'ini_subdistrict.x_status'
go

/*==============================================================*/
/* Table: mem_education                                         */
/*==============================================================*/
create table mem_education (
   member_code          varchar(30)          not null,
   rec_no               int                  not null,
   degree               nvarchar(100)        not null,
   colledge_name        nvarchar(500)        null,
   faculty              nvarchar(500)        null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_educ check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_education primary key (rec_no, member_code)
)
go

execute sp_bindefault d_n, 'mem_education.x_status'
go

/*==============================================================*/
/* Table: mem_group                                             */
/*==============================================================*/
create table mem_group (
   mem_group_code       char(3)              not null,
   mem_group_desc       nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_grou check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_group primary key (mem_group_code)
)
go

execute sp_bindefault d_00, 'mem_group.mem_group_code'
go

execute sp_bindefault d_n, 'mem_group.x_status'
go

/*==============================================================*/
/* Table: mem_health                                            */
/*==============================================================*/
create table mem_health (
   member_code          varchar(30)          not null,
   medical_history      nvarchar(500)        null,
   blood_group          char(1)              null,
   hobby                nvarchar(500)        null,
   restrict_food        nvarchar(500)        null,
   special_skill        nvarchar(500)        null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_heal check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_health primary key (member_code)
)
go

execute sp_bindefault d_n, 'mem_health.x_status'
go

/*==============================================================*/
/* Table: mem_level                                             */
/*==============================================================*/
create table mem_level (
   mlevel_code          char(3)              not null,
   mlevel_desc          nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_leve check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_level primary key (mlevel_code)
)
go

execute sp_bindefault d_00, 'mem_level.mlevel_code'
go

execute sp_bindefault d_n, 'mem_level.x_status'
go

/*==============================================================*/
/* Table: mem_reward                                            */
/*==============================================================*/
create table mem_reward (
   member_code          varchar(30)          not null,
   rec_no               int                  not null,
   reward_desc          nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_rewa check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_reward primary key (rec_no, member_code)
)
go

execute sp_bindefault d_n, 'mem_reward.x_status'
go

/*==============================================================*/
/* Table: mem_site_visit                                        */
/*==============================================================*/
create table mem_site_visit (
   member_code          varchar(30)          not null,
   rec_no               int                  not null,
   site_visit_desc      nvarchar(500)        not null,
   country_code         int                  null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_site check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_site_visit primary key (rec_no, member_code)
)
go

execute sp_bindefault d_n, 'mem_site_visit.x_status'
go

/*==============================================================*/
/* Table: mem_social                                            */
/*==============================================================*/
create table mem_social (
   member_code          varchar(30)          not null,
   rec_no               int                  not null,
   social_desc          nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_soci check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_social primary key (rec_no, member_code)
)
go

execute sp_bindefault d_n, 'mem_social.x_status'
go

/*==============================================================*/
/* Table: mem_status                                            */
/*==============================================================*/
create table mem_status (
   mstatus_code         char(3)              not null,
   mstatus_desc         nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_stat check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_status primary key (mstatus_code)
)
go

execute sp_bindefault d_00, 'mem_status.mstatus_code'
go

execute sp_bindefault d_n, 'mem_status.x_status'
go

/*==============================================================*/
/* Table: mem_train_record                                      */
/*==============================================================*/
create table mem_train_record (
   course_code          varchar(30)          not null,
   member_code          varchar(30)          not null,
   course_grade         char(1)              null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_trai check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_mem_train_record primary key (course_code, member_code)
)
go

execute sp_bindefault d_1, 'mem_train_record.course_code'
go

execute sp_bindefault d_n, 'mem_train_record.x_status'
go

/*==============================================================*/
/* Table: mem_type                                              */
/*==============================================================*/
create table mem_type (
   mem_group_code       char(3)              not null,
   mem_type_code        char(3)              not null,
   mem_type_desc        nvarchar(100)        not null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_type check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_type primary key (mem_type_code, mem_group_code)
)
go

execute sp_bindefault d_00, 'mem_type.mem_group_code'
go

execute sp_bindefault d_00, 'mem_type.mem_type_code'
go

execute sp_bindefault d_n, 'mem_type.x_status'
go

/*==============================================================*/
/* Table: mem_worklist                                          */
/*==============================================================*/
create table mem_worklist (
   rec_no               int                  not null,
   member_code          varchar(30)          null,
   company_name_th      nvarchar(100)        null,
   company_name_eng     nvarchar(100)        null,
   position_name_th     nvarchar(100)        null,
   position_name_eng    nvarchar(100)        null,
   work_year            char(4)              null,
   office_address       nvarchar(500)        null,
   x_status             char(1)              null
      constraint ckc_x_status_mem_work check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_worklist primary key (rec_no)
)
go

execute sp_bindefault d_n, 'mem_worklist.x_status'
go

/*==============================================================*/
/* Table: member                                                */
/*==============================================================*/
create table member (
   member_code          varchar(30)          not null,
   fname                nvarchar(100)        null,
   lname                nvarchar(100)        null,
   sex                  char(1)              null,
   nationality          char(3)              null,
   mem_photo            varchar(30)          null,
   cid_type             char(1)              null,
   cid_card             varchar(30)          null,
   cid_card_pic         varchar(30)          null,
   birthdate            datetime             null,
   current_age          smallint             null,
   religion             smallint             null,
   place_name           varchar(50)          null,
   marry_status         char(1)              null,
   h_no                 varchar(20)          null,
   lot_no               varchar(20)          null,
   village              nvarchar(50)         null,
   building             nvarchar(50)         null,
   floor                varchar(20)          null,
   room                 varchar(20)          null,
   lane                 nvarchar(50)         null,
   street               nvarchar(50)         null,
   subistrict_code      char(8)              null,
   district_code        char(8)              null,
   province_code        char(8)              null,
   country_code         int                  null,
   zip_code             char(5)              null,
   mstatus_code         char(3)              null,
   mem_type_code        char(3)              null,
   mem_group_code       char(3)              null,
   mlevel_code          char(3)              null,
   zone                 varchar(30)          null,
   latitude             decimal(9,6)         null,
   longitude            dec(9,6)             null,
   texta_address        varchar(200)         null,
   textb_address        varchar(200)         null,
   textc_address        varchar(200)         null,
   tel                  nvarchar(50)         null,
   mobile               nvarchar(50)         null,
   fax                  nvarchar(50)         null,
   social_app_data      nvarchar(500)        null,
   email                nvarchar(100)        null,
   x_status             char(1)              null
      constraint ckc_x_status_member check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_member primary key (member_code)
)
go

execute sp_bindefault d_00, 'member.nationality'
go

execute sp_bindefault d_1, 'member.mem_photo'
go

execute sp_bindefault d_1, 'member.cid_card'
go

execute sp_bindefault d_1, 'member.cid_card_pic'
go

execute sp_bindefault d_00, 'member.mstatus_code'
go

execute sp_bindefault d_00, 'member.mem_type_code'
go

execute sp_bindefault d_00, 'member.mem_group_code'
go

execute sp_bindefault d_00, 'member.mlevel_code'
go

execute sp_bindefault d_1, 'member.zone'
go

execute sp_bindefault d_n, 'member.x_status'
go

/*==============================================================*/
/* Table: pic_image                                             */
/*==============================================================*/
create table pic_image (
   image_code           varchar(30)          not null,
   image_name           nvarchar(50)         null,
   ref_doc_type         varchar(30)          null,
   ref_doc_code         varchar(30)          null,
   image_file           text                 null,
   x_status             char(1)              null
      constraint ckc_x_status_pic_imag check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   constraint pk_pic_image primary key (image_code)
)
go

execute sp_bindefault d_1, 'pic_image.image_code'
go

execute sp_bindefault d_1, 'pic_image.ref_doc_type'
go

execute sp_bindefault d_1, 'pic_image.ref_doc_code'
go

execute sp_bindefault d_n, 'pic_image.x_status'
go

/*==============================================================*/
/* Table: project                                               */
/*==============================================================*/
create table project (
   project_code         varchar(30)          not null,
   project_desc         nvarchar(100)        not null,
   project_date         datetime             null,
   project_approve_date datetime             null,
   ref_doc              varchar(30)          null,
   budget               money                null,
   project_manager      nvarchar(100)        null,
   target_member_join   int                  null,
   active_member_join   int                  null,
   passed_member        int                  null,
   x_status             char(1)              null
      constraint ckc_x_status_project check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_project primary key (project_code)
)
go

execute sp_bindefault d_1, 'project.project_code'
go

execute sp_bindefault d_1, 'project.ref_doc'
go

execute sp_bindefault d_0, 'project.budget'
go

execute sp_bindefault d_n, 'project.x_status'
go

/*==============================================================*/
/* Table: project_course                                        */
/*==============================================================*/
create table project_course (
   course_code          varchar(30)          not null,
   project_code         varchar(30)          null,
   ctype_code           char(3)              null,
   cgroup_code          char(3)              null,
   course_desc          nvarchar(100)        not null,
   course_date          datetime             null,
   course_approve_date  datetime             null,
   course_begin         datetime             null,
   course_end           datetime             null,
   ref_doc              varchar(30)          null,
   budget               money                null,
   charge_head          char(10)             null,
   support_head         char(10)             null,
   project_manager      nvarchar(100)        null,
   target_member_join   int                  null,
   active_member_join   int                  null,
   passed_member        int                  null,
   x_status             char(1)              null
      constraint ckc_x_status_project_ check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_project_course primary key (course_code)
)
go

execute sp_bindefault d_1, 'project_course.course_code'
go

execute sp_bindefault d_1, 'project_course.project_code'
go

execute sp_bindefault d_00, 'project_course.ctype_code'
go

execute sp_bindefault d_00, 'project_course.cgroup_code'
go

execute sp_bindefault d_1, 'project_course.ref_doc'
go

execute sp_bindefault d_0, 'project_course.budget'
go

execute sp_bindefault d_n, 'project_course.x_status'
go

/*==============================================================*/
/* Table: project_course_register                               */
/*==============================================================*/
create table project_course_register (
   course_code          varchar(30)          not null,
   member_code          varchar(30)          not null,
   x_status             char(1)              null
      constraint ckc_x_status_project_ check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_project_course_register primary key (course_code, member_code)
)
go

execute sp_bindefault d_1, 'project_course_register.course_code'
go

execute sp_bindefault d_n, 'project_course_register.x_status'
go

/*==============================================================*/
/* Table: project_daily_checklist                               */
/*==============================================================*/
create table project_daily_checklist (
   course_code          varchar(30)          not null,
   member_code          varchar(30)          not null,
   course_date          datetime             not null,
   x_status             char(1)              null
      constraint ckc_x_status_project_ check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_project_daily_checklist primary key (course_date, course_code, member_code)
)
go

execute sp_bindefault d_1, 'project_daily_checklist.course_code'
go

execute sp_bindefault d_n, 'project_daily_checklist.x_status'
go

/*==============================================================*/
/* Table: project_sponsor                                       */
/*==============================================================*/
create table project_sponsor (
   spon_code            varchar(30)          not null,
   spon_desc            nvarchar(100)        not null,
   confirm_date         datetime             null,
   ref_doc              varchar(30)          null,
   contactor            nvarchar(100)        null,
   contactor_detail     nvarchar(500)        null,
   x_status             char(1)              null
      constraint ckc_x_status_project_ check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_project_sponsor primary key (spon_code)
)
go

execute sp_bindefault d_1, 'project_sponsor.spon_code'
go

execute sp_bindefault d_1, 'project_sponsor.ref_doc'
go

execute sp_bindefault d_n, 'project_sponsor.x_status'
go

/*==============================================================*/
/* Table: project_supporter                                     */
/*==============================================================*/
create table project_supporter (
   project_code         varchar(30)          not null,
   spon_code            varchar(30)          not null,
   ref_doc              varchar(30)          null,
   support_budget       money                null,
   contactor            nvarchar(100)        null,
   contactor_detail     nvarchar(500)        null,
   x_status             char(1)              null
      constraint ckc_x_status_project_ check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_project_supporter primary key (spon_code, project_code)
)
go

execute sp_bindefault d_1, 'project_supporter.project_code'
go

execute sp_bindefault d_1, 'project_supporter.spon_code'
go

execute sp_bindefault d_1, 'project_supporter.ref_doc'
go

execute sp_bindefault d_0, 'project_supporter.support_budget'
go

execute sp_bindefault d_n, 'project_supporter.x_status'
go

/*==============================================================*/
/* Table: train_place                                           */
/*==============================================================*/
create table train_place (
   place_code           varchar(30)          not null,
   instructor_desc      nvarchar(100)        not null,
   confirm_date         datetime             null,
   ref_doc              varchar(30)          null,
   contactor            nvarchar(100)        null,
   contactor_detail     nvarchar(500)        null,
   x_status             char(1)              null
      constraint ckc_x_status_train_pl check (x_status is null or (x_status = upper(x_status))),
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   constraint pk_train_place primary key (place_code)
)
go

execute sp_bindefault d_1, 'train_place.place_code'
go

execute sp_bindefault d_1, 'train_place.ref_doc'
go

execute sp_bindefault d_n, 'train_place.x_status'
go

alter table course_instructor
   add constraint fk_course_instructor foreign key (instructor_code)
      references instructor (instructor_code)
go

alter table course_instructor
   add constraint fk_instructor_course foreign key (course_code)
      references project_course (course_code)
go

alter table course_train_place
   add constraint fk_course_place foreign key (place_code)
      references train_place (place_code)
go

alter table course_train_place
   add constraint fk_place_course foreign key (course_code)
      references project_course (course_code)
go

alter table "course_type\"
   add constraint fk_course_group foreign key (cgroup_code)
      references course_group (cgroup_code)
go

alter table ini_district
   add constraint fk_ini_dist_prov foreign key (province_code, country_code)
      references ini_province (province_code, country_code)
go

alter table ini_list_zip
   add constraint fk_inizip_subd foreign key (province_code, country_code, district_code, subistrict_code)
      references ini_subdistrict (province_code, country_code, district_code, subistrict_code)
go

alter table ini_province
   add constraint fk_ini_prov_reference_ini_coun foreign key (country_code)
      references ini_country (country_code)
go

alter table ini_subdistrict
   add constraint fk_ini_subd_dist foreign key (district_code, province_code, country_code)
      references ini_district (district_code, province_code, country_code)
go

alter table mem_education
   add constraint fk_mem_educ foreign key (member_code)
      references member (member_code)
go

alter table mem_health
   add constraint fk_mem_health foreign key (member_code)
      references member (member_code)
go

alter table mem_reward
   add constraint fk_mem_rewa foreign key (member_code)
      references member (member_code)
go

alter table mem_site_visit
   add constraint fk_site_coun foreign key (country_code)
      references ini_country (country_code)
go

alter table mem_site_visit
   add constraint fk_mem_site foreign key (member_code)
      references member (member_code)
go

alter table mem_social
   add constraint fk_mem_soci foreign key (member_code)
      references member (member_code)
go

alter table mem_train_record
   add constraint fk_course_member foreign key (course_code)
      references project_course (course_code)
go

alter table mem_train_record
   add constraint fk_mem_course foreign key (member_code)
      references member (member_code)
go

alter table mem_type
   add constraint fk_mem_group foreign key (mem_group_code)
      references mem_group (mem_group_code)
go

alter table mem_worklist
   add constraint fk_mem_work foreign key (member_code)
      references member (member_code)
go

alter table member
   add constraint fk_member_level foreign key (mlevel_code)
      references mem_level (mlevel_code)
go

alter table member
   add constraint fk_member_reference_ini_list foreign key (province_code, country_code, district_code, subistrict_code, zip_code)
      references ini_list_zip (province_code, country_code, district_code, subistrict_code, zip_code)
go

alter table member
   add constraint fk_mem_status foreign key (mstatus_code)
      references mem_status (mstatus_code)
go

alter table member
   add constraint fk_member_group_type foreign key (mem_type_code, mem_group_code)
      references mem_type (mem_type_code, mem_group_code)
go

alter table project_course
   add constraint fk_course_type foreign key (ctype_code, cgroup_code)
      references "course_type\" (ctype_code, cgroup_code)
go

alter table project_course
   add constraint fk_project__course foreign key (project_code)
      references project (project_code)
go

alter table project_course_register
   add constraint fk_course_regist foreign key (course_code)
      references project_course (course_code)
go

alter table project_course_register
   add constraint fk_member_reg_course foreign key (member_code)
      references member (member_code)
go

alter table project_daily_checklist
   add constraint fk_project_daily_check foreign key (course_code, member_code)
      references project_course_register (course_code, member_code)
go

alter table project_supporter
   add constraint fk_support_project foreign key (spon_code)
      references project_sponsor (spon_code)
go

alter table project_supporter
   add constraint fk_proj_support foreign key (project_code)
      references project (project_code)
go

