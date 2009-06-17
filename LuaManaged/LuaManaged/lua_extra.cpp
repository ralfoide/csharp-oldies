
#if 0

char * strchr(const char * str, int c)
{
	if (str)
		for(; *str; str++)
			if (*str == c)
				return (char *)str;

	return 0;
}

unsigned int strlen(char const *)
{
	return 0;
}


char * strncat(char *,char const *,unsigned int)
{
	return 0;
}

unsigned int strcspn(char const *,char const *)
{
	return 0;
}

double strtod(char const *,char * *)
{
	return 0;
}

char * strcat(char *,char const *)
{
	return 0;
}

char * strcpy(char *,char const *)
{
	return 0;
}

char * strncpy(char *,char const *,unsigned int)
{
	return 0;
}

int abs(int)
{
	return 0;
}

void exit(int)
{
}

void longjmp(int *,int)
{
}

int iscntrl(int)
{
	return 0;
}

int isalpha(int)
{
	return 0;
}

int isspace(int)
{
	return 0;
}

int isdigit(int)
{
	return 0;
}

int isalnum(int)
{
	return 0;
}

int memcmp(void const *,void const *,unsigned int)
{
	return 0;
}

void * memcpy(void *,void const *,unsigned int)
{
	return 0;
}

void * realloc(void *,unsigned int)
{
	return 0;
}

void free(void *)
{
}

int sprintf(char *,char const *,...)
{
	return 0;
}

int strcoll(char const *,char const *)
{
	return 0;
}

#endif
