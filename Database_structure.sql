CREATE SEQUENCE blogger.blogger_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;

create table blogger.users(
	id int8 not null default nextval('blogger.blogger_seq'),
	first_name varchar(150) not null,
	last_name varchar(150),
	username varchar(500),
	email varchar(500),
	"password" varchar(500),
	status varchar(20) not null,
	status_change_date timestamp not null,
	
	constraint users_pk primary key (id)
);

CREATE TABLE blogger.posts (
    id int8 not null default nextval('blogger.blogger_seq'),
    title VARCHAR(255) NOT NULL,
    author_id int8 not null,
    post_content TEXT NOT NULL,
    status varchar(20) not null,
	status_change_date timestamp not null,
	
	constraint posts_pk primary key (id),
	constraint post_auther_fk foreign key (author_id) references blogger.users(id)
);

CREATE TABLE blogger.post_comments (
    id int8 not null default nextval('blogger.blogger_seq'),
    post_id int8 not null,
    user_id int8 not null,
    post_comment text not null,
	status varchar(20) not null,
	status_change_date timestamp not null,
	
	constraint post_comments_pk primary key (id),
	constraint post_fk foreign key (post_id) references blogger.posts(id),
	constraint comment_user_fk foreign key (user_id) references blogger.users(id)
);

CREATE OR REPLACE FUNCTION blogger.getseq()
 RETURNS bigint
 LANGUAGE plpgsql
AS $function$
	BEGIN
		return nextval('blogger.blogger_seq');
	END;
$function$
;
