<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="yes" indent="yes"/>

  <xsl:template match="@* | node()">
	<xsl:copy>
	  <xsl:apply-templates select="@* | node()"/>
	</xsl:copy>
  </xsl:template>

  <xsl:template match="body">
	<body>
	  <table>
		<tbody>
		  <xsl:apply-templates/>
		</tbody>
	  </table>
	</body>
  </xsl:template>

  <xsl:template match="table[tr[td[h2]]]">
	<xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="tr">
	<tr>
	  <xsl:apply-templates select="@* | node()"/>
	</tr>
  </xsl:template>

  <xsl:template match="td[h2]">
	<th>
	  <xsl:if test="current()/h2='Video' or current()/h2='Audio'">
		<xsl:attribute name="class">avhdr</xsl:attribute>
	  </xsl:if>
	  <xsl:attribute name="colspan">2</xsl:attribute>
	  <xsl:value-of select="current()/h2" />
	</th>
  </xsl:template>

  <!-- All content in italic tag are field names -->
  <xsl:template match="td/i">
		<xsl:attribute name="class">fieldName</xsl:attribute>
		<xsl:value-of select="current()" />
  </xsl:template>

  <!-- All content in td tags with a colspan attribute are field values -->
  <xsl:template match="td[@colspan]">
	<td>
		<xsl:attribute name="class">fieldValue</xsl:attribute>
		<xsl:apply-templates/>
	</td>
  </xsl:template>

  <!-- Throw out width attributes -->
  <xsl:template match="@width">
	<xsl:apply-templates />
  </xsl:template>

  <!-- Throw out br tags -->
  <xsl:template match="br">
	<xsl:apply-templates/>
  </xsl:template>

</xsl:stylesheet>
